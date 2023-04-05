using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//if you type /// in Visual Studio, it will automatically expand to a <summary>
/// <summary>
/// Keeps a GameObject on screen.
/// Note that this ONLY works for an orthographic Main Camera
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eType { center, inset, outset };
    public enum eScreenLocs {
        onScreen = 0,
        offRight = 1,
        offLeft = 2,
        offUp = 4,
        offDown = 8,
    }

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public float camWidth;
    public float camHeight;

    void Awake() {
        camHeight = Camera.main.orthographicSize; // b
        camWidth = camHeight * Camera.main.aspect; // c
    }

    void LateUpdate() { // d
        //Find the checkRadius taht will enable center, inset, or outset
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;

        // Restrict the X posiiton to camWidth
        if (pos.x > camWidth + checkRadius) { // e
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }

        if (pos.x < -camWidth - checkRadius) { // e
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }

        // Restrict the Y position to camHeight
        if (pos.y > camHeight + checkRadius) { // e
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }

        if (pos.y < -camHeight - checkRadius) { // e
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }

        if (keepOnScreen && !isOnScreen) {
            transform.position = pos;
            screenLocs |= eScreenLocs.onScreen;
        }
    }

    public bool isOnScreen {
        get { return (screenLocs == eScreenLocs.onScreen ); }
    }

    public bool LocIs (eScreenLocs checkLoc) {
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ( (screenLocs & checkLoc) == checkLoc );
    }
}
