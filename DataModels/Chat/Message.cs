using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.Chat
{

    /// <summary>
    /// 消息模型
    /// </summary>
    public class MessageModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Message { get; set; }




    }

}
