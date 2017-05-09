using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;
using System.Threading.Tasks;
using CodeKingdom.Models;

namespace CodeKingdom.API
{
    public class EditorHub : Hub
    {
        private FileRepository fileRepository = new FileRepository();
        private static List<User> users = new List<User>();
        private static List<KeyValuePair<bool, string>> colors = new List<KeyValuePair<bool, string>>
        {
            new KeyValuePair<bool, string>(false, "AntiqueWhite"),
            new KeyValuePair<bool, string>(false, "Aqua"),
            new KeyValuePair<bool, string>(false, "Aquamarine"),
            new KeyValuePair<bool, string>(false, "Azure"),
            new KeyValuePair<bool, string>(false, "Beige"),
            new KeyValuePair<bool, string>(false, "Bisque"),
            new KeyValuePair<bool, string>(false, "Blue"),
            new KeyValuePair<bool, string>(false, "BlueViolet"),
            new KeyValuePair<bool, string>(false, "Brown"),
            new KeyValuePair<bool, string>(false, "CadetBlue"),
            new KeyValuePair<bool, string>(false, "Chartreuse"),
            new KeyValuePair<bool, string>(false, "Chocolate"),
            new KeyValuePair<bool, string>(false, "Coral"),
            new KeyValuePair<bool, string>(false, "CornflowerBlue"),
            new KeyValuePair<bool, string>(false, "Crimson"),
            new KeyValuePair<bool, string>(false, "DarkGoldenRod"),
            new KeyValuePair<bool, string>(false, "DarkGreen"),
            new KeyValuePair<bool, string>(false, "DarkMagenta"),
            new KeyValuePair<bool, string>(false, "DeepPink"),
            new KeyValuePair<bool, string>(false, "DarkViolet"),
        };

        public void JoinFile(int fileID)
        {
            string id = Context.ConnectionId;
            string username = Context.User.Identity.Name;
            string color = null;

            foreach (var c in colors)
            {
                if (c.Key)
                {
                    continue;
                }
                color = c.Value;
                int index = colors.IndexOf(c);
                colors[index] = new KeyValuePair<bool, string>(true, c.Value);
                break;
            }

            User user = new User(id, username, color);
            user.Groups.Add(Convert.ToString(fileID));
            users.Add(user);

            Groups.Add(Context.ConnectionId, Convert.ToString(fileID));
            Clients.Group(Convert.ToString(fileID)).UserList(users);
        }

        public void OnChange(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).OnChange(data);
        }

        public void UpdateCursor(object data, int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).UpdateCursor(data);
        }

        public void GetUsers(int fileID)
        {
            Clients.Caller.userList(users);
        }

        public void Save(string content, int fileID)
        {
            FileViewModel viewModel = new FileViewModel
            {
                ID = fileID,
                Content = content
            };

            fileRepository.UpdateContent(viewModel);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string id = Context.ConnectionId;

            User user = users.Where(u => u.ID == id).FirstOrDefault();

            foreach (var color in colors)
            {
                if (color.Value == user.Color)
                {
                    int index = colors.IndexOf(color);
                    colors[index] = new KeyValuePair<bool, string>(false, color.Value);
                    break;
                }
            }

            if (user != null)
            {
                if (users.Remove(user))
                {
                    foreach (var group in user.Groups)
                    {
                        Clients.Group(group).RemoveCursor(Context.ConnectionId);
                        Clients.Group(group).userList(users);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }


        public void LeaveFile(int fileID)
        {
            Clients.Group(Convert.ToString(fileID), Context.ConnectionId).RemoveCursor(Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, Convert.ToString(fileID));
        }
    }

    class User
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public List<string> Groups { get; set; }
        public string Color { get; set; }

        public User (string id, string username, string color)
        {
            this.ID = id;
            this.Username = username;
            this.Groups = new List<string>();
            this.Color = color;
        }
    }
} 