using System.Text.Json.Serialization;

namespace Backend3DForge.Responses
{
    public class UserRightResponse : BaseResponse
    {
        public UserRightResponse(bool success, string? message, Models.User? data) : base(success, message, data != null ? new Right(data) : null)
        {
        }

        public class Right
        {
            [JsonPropertyName("canAdministrateForum")]
            public bool CanAdministrateForum { get; set; }
            [JsonPropertyName("canRetrieveDelivery")]
            public bool CanRetrieveDelivery { get; set; }
            [JsonPropertyName("canModerateCatalog")]
            public bool CanModerateCatalog { get; set; }
            [JsonPropertyName("canAdministrateSystem")]
            public bool CanAdministrateSystem { get; set; }

            public Right(Models.User user)
            {
                CanAdministrateForum = user.CanAdministrateForum;
                CanRetrieveDelivery = user.CanRetrieveDelivery;
                CanModerateCatalog = user.CanModerateCatalog;
                CanAdministrateSystem = user.CanAdministrateSystem;
            }
        }
    }
}
