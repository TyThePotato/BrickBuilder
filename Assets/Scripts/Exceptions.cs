using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.Exceptions
{
    public class InspectorValueMismatchException : Exception
    {
        public InspectorValueMismatchException()
        {
            
        }

        public InspectorValueMismatchException(string message) : base(message)
        {
            
        }

        public InspectorValueMismatchException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
    
    // thrown when an invalid asset id is assigned to a brick's "model" property
    public class InvalidAssetException : Exception
    {
        public InvalidAssetException()
        {
            
        }

        public InvalidAssetException(string message) : base(message)
        {
            
        }

        public InvalidAssetException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
    
}