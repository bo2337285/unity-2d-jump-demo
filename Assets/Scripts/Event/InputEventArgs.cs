using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputEventArgs : System.EventArgs {
    public bool jumpPressed, crouchPressed;
    public float horizontalmove;
}