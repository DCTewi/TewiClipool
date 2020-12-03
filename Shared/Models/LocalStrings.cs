namespace TewiClipool.Shared.Models
{
    public static class LocalStrings
    {
        public static class Navagation
        {
            public const string SiteName = "字幕组工作表";

            public const string BlackBoard = "公告";
            public const string Clips = "项目";
            public const string Status = "统计";
            public const string Dashboard = "仪表盘";
            public const string Role = "角色";
            public const string RegisterToken = "注册码";
        }
        public static class LoadMessage
        {
            public const string Loading = "加载中";
            public const string Fetching = "获取中";
            public const string Authorizing = "认证中";
            public const string CompletingLogin = "正在完成登陆";
            public const string CompletingLogout = "正在完成登出";
            public const string LoggingIn = "正在登陆";
            public const string LogOut = "正在登出";
            public const string Registering = "正在注册";
            public const string LoadingUserProfile = "正在加载用户信息";
        }

        public static class Error
        {
            public const string AccessDenied401 = "401 Access Denied";
            public const string AccessDenied401Details = "没有获取当前内容的权限";
            public const string NotFound404 = "404 Not Found";
            public const string NotFound404Details = "请求的内容不存在";
            public const string BadRequest400 = "400 Bad Request";
            public const string LoginFailed = "登陆失败";
            public const string LogOutFailed = "登出失败";
        }

        public static class EmptyMessage
        {
            public const string Nothing = "无匹配项";
            public const string NoStaff = "暂无人接受";
        }

        public static class RemotePage
        {
            public const string Register = "注册";
            public const string RegisterToken = "注册码";
            public const string NewNickName = "默认昵称(可修改)";
            public const string NewUserName = "用户名(中文/英文/数字, 不可修改)";
            public const string NewEmail = "邮箱(形如a@b, 仅作为登录凭证)";
            public const string NewPassword = "密码(6位及以上,至少包含大小写字母和数字)";
            public const string ConfirmPassword = "确认密码";
            public const string RememberMe = "记住我";
            public const string Login = "登录";
            public const string ChangePassword = "修改密码";
            public const string ManageAccount = "管理账户";
            public const string Profile = "个人资料";
            public const string UserName = "用户名";
            public const string NickName = "昵称";
            public const string Email = "邮箱";
            public const string Password = "密码";
            public const string CurrentPassword = "当前密码";
        }

        public static class Type
        {
            public const string ClipType = "切片类型";
            public const string PriorityType = "优先级";
            public const string StaffType = "职位";
            public const string CommentType = "评论类型";
        }

        public static class FilterGroup
        {
            public const string My = "我参与的";
            public const string All = "重置筛选";
        }

        public static class TableHeader
        {
            public const string Id = "Id";
            public const string Token = "注册码";
            public const string Available = "可用性";
            public const string UserName = "用户名";
            public const string Operation = "操作";
            public const string LastComplete = "上次完成";
            public const string LastMonthComplete = "上月完成";
            public const string TotalComplete = "共计完成";
            public const string NickName = "默认昵称";
            public const string Email = "邮箱";
            public const string Skills = "技能点";
        }

        public const string Topped = "置顶";

        public const string Submit = "提交";
        public const string Create = "创建";
        public const string Update = "编辑";
        public const string Delete = "删除";
        public const string CreateMulti = "多个";
        public const string CreateMultiRandom = "随机";

        public const string Title = "标题";
        public const string Content = "内容";
        public const string ContentHTML = "内容(HTML)";
        public const string Minute = "分钟";
        public const string OriginInfo = "出典";
        public const string Length = "长度";
        public const string Description = "简介";
        public const string StaffDescription = "负责";
        public const string Attend = "参加";
        public const string Comment = "评论";
        public const string Edited = "已编辑";

        public const string Register = "注册";
        public const string Login = "登录";
        public const string Logout = "登出";
        public const string Filter = "筛选";
        public const string Keyword = "关键词";
        public const string Count = "数量";

        public const string Available = "可用";
        public const string Used = "已用";
        public const string Sage = "锤人";
        public const string GiveUp = "放弃";
        public const string Complete = "完成";
        public const string Redo = "重做";

        public const string AcceptTime = "接受时间";
        public const string NickName = "昵称";
        public const string UserName = "用户名";
        public const string Source = "源地址";
        public const string Status = "状态";
        public const string Operations = "操作";
        public static string GetClipType(ClipType type)
        {
            return type switch
            {
                ClipType.LiveClip => "杂谈",
                ClipType.SongClip => "歌回",
                ClipType.FullLive => "全熟",
                ClipType.PV => "PV企划",
                ClipType.Collabo => "联动",
                _ => "其他",
            };
        }

        public static string GetCommentType(CommentType type)
        {
            return type switch
            {
                CommentType.Chat => "吹水",
                CommentType.Notice => "公示",
                CommentType.Warning => "警告",
                _ => "其他",
            };
        }

        public static string GetFilterType(FilterType type)
        {
            return type switch
            {
                FilterType.ByTitle => "标题",
                FilterType.ByOriginInfo => "出典",
                FilterType.ByCreator => "发布者",
                FilterType.ByAttender => "参加者",
                _ => "",
            };
        }

        public static string GetPriorityType(PriorityType type)
        {
            return type switch
            {
                PriorityType.Important => "待激光",
                PriorityType.Normal => "待加工",
                PriorityType.Completed => "已完成",
                PriorityType.Sage => "已降权",
                PriorityType.Desperated => "已过时",
                _ => "未知",
            };
        }

        public static string GetProcessType(ProcessType type)
        {
            return type switch
            {
                ProcessType.Accepted => "接受",
                ProcessType.Completed => "完成",
                ProcessType.GaveUp => "放弃",
                _ => "其他",
            };
        }

        public static string GetStaffType(StaffType type)
        {
            return type switch
            {
                StaffType.Editor => "剪辑",
                StaffType.Translator => "翻译",
                StaffType.Timeline => "时轴",
                StaffType.Reviewer => "校对",
                StaffType.PostProducer => "后期",
                StaffType.Art => "美术",
                _ => "其他",
            };
        }
    }
}
