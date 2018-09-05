using UnityEngine;
//Add This script
using System.Collections;
using System.Collections.Generic;
namespace Vuforia
{
    /// &lt;summary&gt;
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// &lt;/summary&gt;
    public class DefaultTrackableEventHandler : MonoBehaviour,
    ITrackableEventHandler
    {
        //------------Begin Sound----------
        public AudioSource soundTarget;
        public AudioClip clipTarget;
        private AudioSource[] allAudioSources;
        public Transform MMDmodel;
        public Animator animator;
        //function to stop all sounds
        void pauseAudio()
        {
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.Pause();
            }
        }
        //function to play sound
        void playSound(string ss)
        {
            clipTarget = (AudioClip)Resources.Load(ss);
            soundTarget.clip = clipTarget;
            soundTarget.loop = true;
            soundTarget.playOnAwake = true;
            soundTarget.Play();
        }
        //-----------End Sound------------
        #region PRIVATE_MEMBER_VARIABLES
        private TrackableBehaviour mTrackableBehaviour;
        #endregion // PRIVATE_MEMBER_VARIABLES

        #region UNTIY_MONOBEHAVIOUR_METHODS
        void Start()
        {
            animator = MMDmodel.GetComponent<Animator>();

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
            //Register / add the AudioSource as object
            soundTarget = (AudioSource)gameObject.AddComponent<AudioSource>();
        }

        public void playAnim()
        {
            animator.Play("TDA Black Neru_arm|Motion_bone");
            animator.speed = 1f;
        }

        public void pauseAnim(){
            animator.speed = 0f;
        }
        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS
        /// &lt;summary&gt;
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// &lt;/summary&gt;
        public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }
        #endregion // PUBLIC_METHODS

        #region PRIVATE_METHODS
        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }
            Debug.Log("Trackable" +mTrackableBehaviour.TrackableName + " found ");
            //Play Sound, IF detect an target
                playSound("Songs/wav faded");
                playAnim();
        }
        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }
            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }
            Debug.Log("Trackable" +mTrackableBehaviour.TrackableName +"lost");
            //Stop All Sounds if Target Lost
            pauseAudio();
            pauseAnim();
        }
        #endregion // PRIVATE_METHODS
    }
}