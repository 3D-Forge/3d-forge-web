﻿using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class ModelCategoryResponse : BaseResponse
    {
        public ModelCategoryResponse(ModelCategory model) : base(true, null, null)
        {
            Data = new View(model);
        }

        public class View
        {
            public int Id { get; set; }
            public string Name { get; set; }


            public View(ModelCategory model)
            {
                this.Id = model.Id;
                this.Name = model.ModelCategoryName;
            }
        }
    }
}
