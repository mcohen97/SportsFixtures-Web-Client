using System;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DataRepositoriesTest")]

namespace BusinessLogic
{
    public class Team
    {
        private string name;
        private string photo;
        private int id;
        
        public Team(string name, string photo) {
            Name = name;
            Photo = photo;
        }

        public Team(int id, string name, string photo):this(name,photo)
        {
            Id = id;
        }

        public string Name {get{return name;} set{SetName(value);}}

        public string Photo {get{return photo;} set{SetPhoto(value);}}

        public int Id { get{return id;} set{SetId(value);} }

        public void SetId(int value){
            id = value;
        }

        private void SetName(string value)
        {
            if(value == null)
                throw new InvalidTeamDataException("Name can't be null");

            if(value == "")
                throw new InvalidTeamDataException("Name can't be empty");
            
            name = value;
        }

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
            return Id == team.Id;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}