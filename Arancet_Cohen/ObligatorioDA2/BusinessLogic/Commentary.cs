using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic
{
    public class Commentary
    {
        private int id;
        private string text;

        public Commentary(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public int Id { get;  set; }
        public string Text { get{return text;}  set{SetText(value);} }

        private void SetText(string value)
        {
            if(value == null)
                throw new InvalidCommentaryDataException("Text can't be null");

            if(value == "")
                throw new InvalidCommentaryDataException("Text can't be empty");

            text = value;
        }

        public override bool Equals(object obj){
            if(obj == null)
                return false;
            if(!(obj is Commentary))
                return false;
            
            Commentary commentaryCompared = (Commentary) obj;
            
            return Id == commentaryCompared.Id;
        }
    }
}