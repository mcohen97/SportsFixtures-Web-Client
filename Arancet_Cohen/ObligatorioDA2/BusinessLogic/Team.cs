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
    }
}