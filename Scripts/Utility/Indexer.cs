using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satisfy
{
    public static class Indexer
    {
        static int index;

        public static int GetIndex()
        {
            index++;
            return index;
        }
    }
}