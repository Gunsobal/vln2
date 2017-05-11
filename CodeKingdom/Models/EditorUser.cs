using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models
{

    public class EditorUser
    {
        /// <summary>
        /// ConnectionId from Signal R goes here in
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Email address of the logged in user
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Signal R groups that this user is subscribed to
        /// </summary>
        public List<string> Groups { get; set; }

        /// <summary>
        /// Color of this user. Used in the editor to distinguish him from other users visually
        /// </summary>
        public string Color { get; set; }

        public EditorUser (string id, string username)
        {
            this.ID = id;
            this.Username = username;
            this.Groups = new List<string>();
            this.Color = GetColor();
        }

        /// <summary>
        /// User who has this color has disconnected and is not using it anymore. 
        /// The color needs to be clear'd so it can be used again
        /// </summary>
        /// <param name="userColor"></param>
        public static void ClearColor(string userColor)
        {
            foreach (var color in colors)
            {
                if (color.Value == userColor)
                {
                    int index = colors.IndexOf(color);
                    colors[index] = new KeyValuePair<bool, string>(false, color.Value);
                    break;
                }
            }
        }


        /// <summary>
        /// Users are marked by color in the editor. This are available colors, they are also marked which one is in use.
        /// Maybe not the best idea to have x number of colors if the system has more then x active users
        /// TODO: Needs to be refactored for better scalability
        /// </summary>
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

        /// <summary>
        /// Gets next available color in the list. After the color has been found it is marked as in use.
        /// </summary>
        /// <returns>Color name</returns>
        private static string GetColor()
        {
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

            return color;
        }
    }
}