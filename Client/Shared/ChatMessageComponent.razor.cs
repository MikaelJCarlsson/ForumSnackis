using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ForumSnackis.Client.Shared
{
    public partial class ChatMessageComponent : ComponentBase
    {

        [Parameter]
        public MessageDTO Message { get; set; }
        [Parameter]
        public string CurrentUserID { get; set; }

    }

}
