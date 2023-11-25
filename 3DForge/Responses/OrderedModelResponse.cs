namespace Backend3DForge.Responses
{
	public class OrderedModelResponse : BaseResponse
	{
		public OrderedModelResponse(Models.OrderedModel model) : base(true, null, null)
		{
			Data = new OrderedModel(model);
		}

		public class OrderedModel
		{
			public int Id { get; set; }
			public string PrintExtensionName { get; set; }
			public double PricePerPiece { get; set; }
			public int Pieces { get; set; }
			public float XSize { get; set; }
			public float YSize { get; set; }
			public float ZSize { get; set; }
			public double Volume { get; set; } = 0;
			public double Scale { get; set; } = 1;
			public float Depth { get; set; }
			public string Color { get; set; }
			public string PrintTypeName { get; set; }
			public string PrintMaterialName { get; set; }

			public OrderedModel(Models.OrderedModel model)
			{
				Id = model.Id;
				PrintExtensionName = model.PrintExtensionName;
				PricePerPiece = model.PricePerPiece;
				Pieces = model.Pieces;
				XSize = model.XSize;
				YSize = model.YSize;
				ZSize = model.ZSize;
				Volume = model.Volume;
				Scale = model.Scale;
				Depth = model.Depth;
				Color = model.Color;
				PrintTypeName = model.PrintTypeName;
				PrintMaterialName = model.PrintMaterialName;
			}
		}
	}
}
