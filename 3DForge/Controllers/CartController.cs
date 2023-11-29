using Azure.Core;
using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.BackgroundWorker;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Services.ModelCalculator;
using Backend3DForge.Services.TempStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using static Backend3DForge.Responses.OrderedModelResponse;

namespace Backend3DForge.Controllers
{

    [ApiController]
    [Route("api/cart")]
    public class CartController : BaseController
    {
        protected readonly IFileStorage fileStorage;
        protected readonly IModelCalculator modelCalculator;
        protected readonly IConfiguration configuration;
        protected readonly ITempStorage tempStorage;
        protected readonly IBackgroundWorker backgroundWorker;

        private float MinCost => configuration.GetValue<float>("MinCost");
        private float ValueAdded => configuration.GetValue<float>("ValueAdded");

        public CartController(DbApp db,
                              IFileStorage fileStorage,
                              IModelCalculator modelCalculator,
                              IConfiguration configuration,
                              ITempStorage tempStorage,
                              IBackgroundWorker backgroundWorker) : base(db)
        {
            this.fileStorage = fileStorage;
            this.modelCalculator = modelCalculator;
            this.configuration = configuration;
            this.tempStorage = tempStorage;
            this.backgroundWorker = backgroundWorker;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var cart = await DB.Carts
                .Include(p => p.OrderedModels)
                .FirstOrDefaultAsync(x => x.UserId == AuthorizedUser.Id);

            if (cart is null)
            {
                return Ok(new BaseResponse.SuccessResponse("No items in the cart", new OrderedModelResponse.OrderedModel[] { }));
            }

            return Ok(new CartResponse(cart));
        }

        [Authorize]
        [HttpPut]
        public async Task AddItem([FromForm] CartAddRequest addRequest)
        {
            string? addRequestFileId = null;

            var printTypeName = await DB.PrintTypes.FirstOrDefaultAsync(x => x.Name == addRequest.PrintTypeName);
            if (printTypeName is null)
            {
                BadRequestVoid(new BaseResponse.ErrorResponse("This print type does not exists"));
                return;
            }

            var printMaterialName = await DB.PrintMaterials.FirstOrDefaultAsync(x => x.Name == addRequest.PrintMaterialName);
            if (printMaterialName is null)
            {
                BadRequestVoid(new BaseResponse.ErrorResponse("This print material does not exists"));
                return;
            }

            var cart = await DB.Carts.FirstOrDefaultAsync(x => x.UserId == AuthorizedUser.Id);

            if (cart is null)
            {
                cart = new Models.Cart()
                {
                    CreatedAt = DateTime.UtcNow,
                    UserId = AuthorizedUser.Id
                };
                cart = (await DB.Carts.AddAsync(cart)).Entity;
                await DB.SaveChangesAsync();
            }

            if (addRequest.CatalogModelId is not null)
            {
                var catalogModel = await DB.CatalogModels.FirstOrDefaultAsync(x => x.Id == addRequest.CatalogModelId);

                if (catalogModel is null)
                {
                    BadRequestVoid(new BaseResponse.ErrorResponse("Model with chosen ID does not exists"));
                    return;
                }

                var orderedModel = new Models.OrderedModel()
                {
                    CartId = cart.Id,
                    CatalogModelId = catalogModel.Id,
                    PrintExtensionName = catalogModel.PrintExtensionName,
                    PricePerPiece = catalogModel.MinPrice * (1 + (addRequest.Depth / 100f)),
                    Pieces = addRequest.Pieces,
                    XSize = catalogModel.XSize,
                    YSize = catalogModel.YSize,
                    ZSize = catalogModel.ZSize,
                    Depth = addRequest.Depth,
                    Scale = addRequest.Scale,
                    Color = addRequest.Color,
                    PrintTypeName = printTypeName.Name,
                    PrintMaterialName = printMaterialName.Name,
                };

                await DB.OrderedModels.AddAsync(orderedModel);
                await DB.SaveChangesAsync();

                OkVoid(new OrderedModelResponse(orderedModel));
                return;
            }
            else if (addRequest.File is not null)
            {
                string printExtenstion = Path.GetExtension(addRequest.File.FileName).Replace(".", "");

                if (addRequest.File.Length > 1024 * 1024 * 50)
                {
                    BadRequestVoid(new BaseResponse.ErrorResponse("Print file is too large. Max size: 50MB"));
                    return;
                }

                var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.Name == printExtenstion);

                if (printExtension is null)
                {
                    BadRequestVoid(new BaseResponse.ErrorResponse("Invalid file format for printing"));
                    return;
                }

                addRequestFileId = await this.tempStorage.UploadFileAsync(addRequest.File.OpenReadStream());

                var taskInfo = this.backgroundWorker.CreateTask((args) =>
                {
                    DbApp db = args[0] as DbApp ?? throw new ArgumentNullException();
                    ITempStorage tempStorage = args[1] as ITempStorage ?? throw new ArgumentNullException();
                    IFileStorage fileStorage = args[2] as IFileStorage ?? throw new ArgumentNullException();
                    IModelCalculator modelCalculator = args[3] as IModelCalculator ?? throw new ArgumentNullException();
                    string[] printFileInfo = args[4] as string[] ?? throw new ArgumentNullException();
                    PrintType printTypeName = args[5] as PrintType ?? throw new ArgumentNullException();
                    PrintMaterial printMaterialName = args[6] as PrintMaterial ?? throw new ArgumentNullException();
                    CartAddRequest cartAddRequest = args[7] as CartAddRequest ?? throw new ArgumentNullException();

                    ModelCalculatorResult printParameters;
                    using (var fs = tempStorage.DownloadFileAsync(printFileInfo[1]))
                    {
                        try
                        {

                            printParameters = modelCalculator.CalculateSurfaceArea(fs, printFileInfo[0]);

                        }
                        catch (Exception ex)
                        {
                            return new BaseResponse.ErrorResponse(ex.Message);
                        }

                        var cheapestMaterialCost = db.PrintMaterials.Min(x => x.Cost);

                        var price = cheapestMaterialCost * (printParameters.Volume * Math.Pow(cartAddRequest.Scale, 3) / 1000f);

                        price = price * (1 + (cartAddRequest.Depth / 100f));

                        if (price < MinCost)
                        {
                            price = MinCost;
                        }
                        else
                        {
                            price += ValueAdded;
                        }

                        var orderedModel = new Models.OrderedModel()
                        {
                            CartId = cart.Id,
                            PrintExtensionName = printFileInfo[0],
                            PricePerPiece = price,
                            Pieces = cartAddRequest.Pieces,
                            XSize = printParameters.X,
                            YSize = printParameters.Y,
                            ZSize = printParameters.Z,
                            Depth = cartAddRequest.Depth,
                            Scale = cartAddRequest.Scale,
                            Color = cartAddRequest.Color,
                            PrintTypeName = printTypeName.Name,
                            PrintMaterialName = printMaterialName.Name,
                        };

                        db.OrderedModels.Add(orderedModel);
                        db.SaveChanges();

                        fileStorage.UploadPrintFile(orderedModel, fs).Wait();

                        return new OrderedModelResponse(orderedModel);
                    }
                }, new object[]
                {
                    DB, tempStorage, fileStorage, modelCalculator,
                    new string[]
                    {
                        printExtension.Name,
                        addRequestFileId
                    },
                    printTypeName,
                    printMaterialName,
                    addRequest
                });
                await backgroundWorker.SubscribeToTaskInformation(taskInfo.Id, Response);
            }
            else
            {
                BadRequestVoid(new BaseResponse.ErrorResponse("Chose something one!"));
                return;
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveItem(int orderedModelId)
        {
            var cart = await DB.Carts
                .Include(p => p.OrderedModels)
                .FirstOrDefaultAsync(x => x.UserId == AuthorizedUser.Id);

            if (cart is null)
            {
                return BadRequest(new BaseResponse.SuccessResponse("No items in the cart"));
            }

            var orderedModel = cart.OrderedModels.FirstOrDefault(x => x.Id == orderedModelId);

            if (orderedModel is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected ordered model does not exists"));
            }

            DB.OrderedModels.Remove(orderedModel);

            await DB.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse("Model has been removed from the cart"));
        }
    }
}
