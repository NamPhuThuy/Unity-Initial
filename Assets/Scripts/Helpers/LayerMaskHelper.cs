using UnityEngine;

namespace NamPhuThuy.Common
{
    public class LayerMaskHelper
    {
        public static int OnlyIncluding(params int[] layers)
        {
            return MakeMask(layers);
        }

        public static int Everything()
        {
            return -1;
        }

        public static int Default()
        {
            return 1;
        }

        public static int Nothing()
        {
            return 0;
        }

        public static int EverythingBut(params int[] layers)
        {
            return ~MakeMask(layers);
        }

        public static bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (layerMask.value & 1 << layer) != 0;
        }

        static int MakeMask(params int[] layers)
        {
            int mask = 0;
            foreach (int item in layers)
            {
                mask |= 1 << item;
            }
            return mask;
        }
    }
}

/*
 // Create a mask including layers 3 and 6
    int mask = LayerMaskHelper.OnlyIncluding(3, 6);
    
     
    // Assign the mask to a LayerMask
        LayerMask layerMask = mask;

    // Check if layer 3 is in the mask
    bool containsLayer3 = LayerMaskHelper.ContainsLayer(layerMask, 3);
    Debug.Log("Mask contains layer 3: " + containsLayer3);

    // Get a mask for everything except layer 2
    int everythingButLayer2 = LayerMaskHelper.EverythingBut(2);
    Debug.Log("Everything but layer 2 mask: " + everythingButLayer2);
    
 */