using System;
using System.Collections.Generic;
using BusinessLogicExceptions;

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
            this.Name = name;
            this.Photo = photo;
        }

        public string Name {get{return name;} set{SetName(value);}}

        private void SetName(string value)
        {
            if(value == null)
                throw new InvalidTeamDataException("Name can't be null");

            if(value == "")
                throw new InvalidTeamDataException("Name can't be empty");
            
            name = value;
        }

        public string Photo {get{return photo;} set{SetPhoto(value);}}

        private void SetPhoto(string value)
        {
            if(value == null)
                throw new InvalidTeamDataException("Photo can't be null");

            photo = value;       
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