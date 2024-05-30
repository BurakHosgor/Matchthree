using Events;
using UnityEngine;
using Zenject;

namespace Components
{
   public class TestCube : MonoBehaviour
   {
      [Inject] private ProjectEvents ProjectEvents { get; set; }


      private void OnEnable()
      {
         RegisterEvent();
      }

      private void OnDisable()
      {
         UnRegisterEvent();
      }

      void RegisterEvent()
      {
         ProjectEvents.ProjectStarted += OnProjectInstalled;
      }


      void OnProjectInstalled()
      {
         Debug.LogWarning("Var");
      }

      void UnRegisterEvent()
      {
         ProjectEvents.ProjectStarted -= OnProjectInstalled;
      }
   }

}