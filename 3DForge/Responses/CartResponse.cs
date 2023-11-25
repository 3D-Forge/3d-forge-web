using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Responses
{
	public class CartResponse : BaseResponse
	{
		public CartResponse(Models.Cart cart) : base(true, null, null)
		{
			Data = new Cart(cart);
		}

		public class Cart
		{
			[Key]
			public int Id { get; set; }
			[Required]
			public OrderedModelResponse.OrderedModel[] OrderedModelIDs { get; set; }

			public Cart(Models.Cart cart)
			{
				Id = cart.Id;
				OrderedModelIDs = cart.OrderedModels.Select(x => new OrderedModelResponse.OrderedModel(x)).ToArray();
			}
		}
	}
}
