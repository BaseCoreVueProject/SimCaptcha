﻿using System.Collections.Generic;
// Project: SimCaptcha
// https://github.com/yiyungent/SimCaptcha
// Author: yiyun <yiyungent@gmail.com>

namespace SimCaptcha.Models
{
    public class VCodeResponseModel
    {
        /// <summary>
        /// 0 成功
        /// </summary>
        public int code { get; set; }

        public string message { get; set; }

        public DataModel data { get; set; }

        public class DataModel
        {
            /// <summary>
            /// 用户此次会话唯一标识
            /// </summary>
            public string userId { get; set; }

            /// <summary>
            /// 验证图片base64
            /// </summary>
            public string vCodeImg { get; set; }

            /// <summary>
            /// 验证提示
            /// </summary>
            public string vCodeTip { get; set; }

            /// <summary>
            /// 答案: 字(有顺序 eg: 望,我,哈,他),  也可以为空, 目前前端只用vCodeTip,无用,保留 
            /// </summary>
            public IList<string> words { get; set; }
        }
    }
}
