﻿using Microsoft.AspNetCore.Authorization;

namespace Backend3DForge.Attributes
{
	public class CanModerateCatalogAttribute : AuthorizeAttribute
	{
		public CanModerateCatalogAttribute()
		{
		}
	}
}
