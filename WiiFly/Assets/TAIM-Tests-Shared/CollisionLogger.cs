using System.IO;
using UnityEngine;

public class CollisionLogger : MonoBehaviour
{
    [SerializeField] private string loggerName = "CollisionLogger";
    private const string FILE_NAME = "log.csv";

    private void OnCollisionEnter(Collision collision)
    {
        string message = string.Format("{0},{1},{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), loggerName, collision.gameObject.name);
        File.AppendAllText(FILE_NAME, message + "\n");
    }
}