using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, ~LayerMask.GetMask("Wall")))
            {
                //Debug.Log("Raycast hit: " + hit.collider.name);

                // Check if the object hit has the KeypadButton script
                if (hit.collider.TryGetComponent<KeypadButton>(out var button))
                {
                    button.InteractStart(); // Or whatever your function is
                }
                // Check if the object hit has the ElectricalCircuitComponent script
                if (hit.collider.TryGetComponent<ElectricalCircuitComponent>(out var button2))
                {
                    button2.InteractStart(); // Or whatever your function is
                }
            }
        }
    }
 
}
