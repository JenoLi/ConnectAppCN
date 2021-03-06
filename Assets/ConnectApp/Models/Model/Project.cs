using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class Project {
        public Article projectData;
        public List<string> pureContentIds;
        public List<Article> projects;
        public Dictionary<string, ContentMap> contentMap;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
        public FetchCommentsResponse comments;
        public string channelId;
        public bool like;
        public bool edit;
    }
}