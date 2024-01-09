using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonInfo.Service
{
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string name, object key)
            : base($"Entity: {name} - (მომხმარებელი: { key } მონაცემთა ბაზაში უკვე არსებობს.")
        {
        }
    }
}
