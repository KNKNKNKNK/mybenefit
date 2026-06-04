using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MachineData;
using ItemData;

public class MachineState : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI MachineIdText;

    [SerializeField]
    Image PowerLight;

    string MachineId = "";
    MachineStatus Status;
    Dictionary<int, ProductData> Products;

    public void SetMachineState(string machineId_, MachineStatus status_, Dictionary<int, ProductData> dicData_)
    {
        MachineId = machineId_;
        Status = status_;
        Products = dicData_;

        MachineIdText.text = MachineId;
        SetPowerLightColor(Status);

        Debug.Log("----------------------");
        foreach (var temp in Products)
        {
            Debug.Log("키" + temp.Key + "  " + temp.Value.name + temp.Value.stock);
        }
    }

    public void SetPowerLightColor(MachineStatus o)
    {
        switch (o)
        {
            case MachineStatus.Active:
                PowerLight.color = Color.green;
                break;
            case MachineStatus.Inactive:
                PowerLight.color = Color.red;
                break;
            default:
                break;
        }
    }
}
