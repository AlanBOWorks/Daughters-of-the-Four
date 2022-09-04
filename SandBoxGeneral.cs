using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Maths;

public class SandBoxGeneral : MonoBehaviour
{


    [Button]
    private void Test(int randomUpToSixteen =8)
    {
        var generator = UtilsRandom.EightSizeNonConsecutiveRandomNumbersGenerator;
        var list = generator.CalculateNonConsecutiveRandoms(randomUpToSixteen);
        foreach (var i in list)
        {
            Debug.Log(i);
        }
    }

}
