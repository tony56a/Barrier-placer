using BarrierPlacer.Manager;
using BarrierPlacer.Utils;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BarrierPlacer
{
    public class BarrierPlacerSerializer : SerializableDataExtensionBase
    {
        private readonly string key = "BarrierPlacer";

        public override void OnSaveData()
        {
            LoggerUtils.Log("Saving Barriers");

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream barrierMemoryStream = new MemoryStream();


            try
            {
                BarrierStrip[] barriers = BarrierManager.Instance().m_barrierList.ToArray();

                if (barriers != null)
                {
                    binaryFormatter.Serialize(barrierMemoryStream, barriers);
                    serializableDataManager.SaveData(key, barrierMemoryStream.ToArray());
                    LoggerUtils.Log("Barriers have been saved!");

                }
                else
                {
                    LoggerUtils.LogWarning("Couldn't save barriers, as the array is null!");
                }

              
            }
            catch (Exception ex)
            {
                LoggerUtils.LogException(ex);
            }
            finally
            {
                barrierMemoryStream.Close();
            }
        }

        public override void OnLoadData()
        {
            LoggerUtils.Log("Loading barriers");

            byte[] loadedBarrierData = serializableDataManager.LoadData(key);

            if (loadedBarrierData != null)
            {
                MemoryStream barrierMemoryStream = new MemoryStream();

                barrierMemoryStream.Write(loadedBarrierData, 0, loadedBarrierData.Length);
                barrierMemoryStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                try
                {
                    BarrierStrip[] barriers = binaryFormatter.Deserialize(barrierMemoryStream) as BarrierStrip[];

                    if (barriers != null)
                    {
                        BarrierManager.Instance().Load(barriers);
                    }
                    else
                    {
                        LoggerUtils.LogWarning("Couldn't load barriers, as the array is null!");
                    }
                }
                catch (Exception ex)
                {
                    LoggerUtils.LogException(ex);

                }
                finally
                {
                    barrierMemoryStream.Close();
                }
            }
            else
            {
                LoggerUtils.LogWarning("Found no data to load");
            }

        }
    }
}
