using System.Collections.Generic;

namespace BusinessLogic
{
    public class Team
    {
        private string name;
        private string photo;

        public Team()
        {
            name = "";
            photo = "";
        }
        public Team(string name, string photo)
        {
            this.name = name;
            this.photo = photo;
        }

        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Photo {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Team))
                return false;

            var team = obj as Team;
            return Name == team.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}