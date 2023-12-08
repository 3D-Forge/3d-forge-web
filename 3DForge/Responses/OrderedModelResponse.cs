namespace Backend3DForge.Responses
{
	public class OrderedModelResponse : BaseResponse
	{
		public OrderedModelResponse(Models.OrderedModel model) : base(true, null, null)
		{
			Data = new View(model);
		}

		public class View
		{
			public int Id { get; set; }
			public int? CatalogModelId { get; set; }
			public string PrintExtensionName { get; set; }
			public double PricePerPiece { get; set; }
			public int Pieces { get; set; }
			public float XSize { get; set; }
			public float YSize { get; set; }
			public float ZSize { get; set; }
			public double Volume { get; set; } = 0;
			public double Scale { get; set; } = 1;
			public float Depth { get; set; }
			public int ColorId { get; set; }
			public string ColorRGB { get; set; }
			public string ColorHex { get; set; }
            public string PrintTypeName { get; set; }
			public string PrintMaterialName { get; set; }

			public double TotalPrice => PricePerPiece * Pieces;

			public View(Models.OrderedModel model)
			{
				Id = model.Id;
				CatalogModelId = model.CatalogModelId;
				PrintExtensionName = model.PrintExtensionName;
				PricePerPiece = model.PricePerPiece;
				Pieces = model.Pieces;
				XSize = model.XSize;
				YSize = model.YSize;
				ZSize = model.ZSize;
				Volume = model.Volume;
				Scale = model.Scale;
				Depth = model.Depth;
				ColorId = model.PrintMaterialColorId;
				ColorRGB = model.PrintMaterialColor.RGB;
				ColorHex = model.PrintMaterialColor.Hex;
				PrintTypeName = model.PrintTypeName;
				PrintMaterialName = model.PrintMaterialName;
			}
		}
	}
}
