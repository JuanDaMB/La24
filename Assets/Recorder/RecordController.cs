using System.Collections;
using Unity_Runtime_Recorder.Scripts.UnityAnimSaver;
using UnityEngine;

namespace Recorder
{
    public class RecordController : MonoBehaviour
    {
        [SerializeField] private Bola _bola;
        [SerializeField] private UnityAnimationRecorder _recorder;
        // Start is called before the first frame update
        void Start()
        {
            // StartCoroutine(recordRoutine());
            // _bola.OnCompleteMove += EndMovimiento;
        }

        private bool CallEnd = false;

        public void EndMovimiento()
        {
            CallEnd = true;
        }

        IEnumerator recordRoutine()
        {
            yield return new  WaitForSeconds(5f);
            
            // for (int i = 0; i < _bola.Datas.Count; i++)
            // {
            //     for (int j = 0; j < _bola.Datas[i].GetArraySize(); j++)
            //     {
            //         _bola.SetNumberExact(i,j);
            //         yield return null;
            //         _recorder.SetName(_bola.Datas[i].number + " " + j);
            //         _recorder.StartRecordingKey();
            //         yield return null;
            //         _bola.Comenzar();
            //         while (!CallEnd)
            //         {
            //             yield return null;
            //         }
            //
            //         if (CallEnd)
            //         {
            //             CallEnd = false;
            //             _recorder.EndRecordingKey();
            //         }
            //
            //         yield return new WaitForSeconds(3f);
            //     }
            //
            //     yield return new WaitForSeconds(3f);
            // }
            yield return null;
        }
    }
}
