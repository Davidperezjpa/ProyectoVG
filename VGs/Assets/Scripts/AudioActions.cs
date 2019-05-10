using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioActions : MonoBehaviour {
    public AudioSource walk, shoot, sword, hook, jump, dash;

    public void PlayWalk() {
        walk.Play();
    }
    public void StopWalk() {
        walk.Stop();
    }
    public void PlayShoot() {
        shoot.Play();
    }
    public void PlaySword() {
        sword.Play();
    }
    public void PlayHook() {
        hook.Play();
    }
    public void PlayJump() {
        jump.Play();
    }
    public void PlayDash() {
        dash.Play();
    }
}
