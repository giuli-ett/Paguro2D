using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("INPUT")]
    [SerializeField] private KeyCode rightInputKey;
    [SerializeField] private KeyCode leftInputKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode runKey;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private KeyCode inventoryKey;
    [SerializeField] private KeyCode selectionKey;
    [SerializeField] private KeyCode camouflageKey;

    private float horizontalInput;
    public bool inputBlock = false;

    public float Horizontal 
    {
        get
        {
            if (inputBlock)
            {
                return 0f;
            }
            else
            {
                return horizontalInput;
            }
        }
    }

    public bool Jump 
    {
        get
        {
            return Input.GetKeyDown(jumpKey);
        }
    }

    public bool Run 
    {
        get
        {
            return Input.GetKey(runKey);
        }
    }

    public bool Dash
    {
        get
        {
            return Input.GetKeyDown(dashKey);
        }
    }

    public bool Inventory
    {
        get
        {
            return Input.GetKeyDown(inventoryKey);
        }
    }

    public bool Select
    {
        get
        {
            return Input.GetKeyDown(selectionKey);
        }
    }
    public bool Camouflage
    {
        get
        {
            return Input.GetKeyDown(camouflageKey);
        }
    }
    private void GetInput()
    {
        if (Input.GetKey(rightInputKey))
        {
            // Debug.Log("[PLAYERINPUT]: Stai andando a destra");
            horizontalInput = 1.0f;
        }
        else if (Input.GetKey(leftInputKey))
        {
            // Debug.Log("[PLAYERINPUT]: Stai andando a sinistra");
            horizontalInput = -1.0f;
        }
        else
        {
            horizontalInput = 0.0f;
        }
    }

    private void Update()
    {
        GetInput();
    }
}
