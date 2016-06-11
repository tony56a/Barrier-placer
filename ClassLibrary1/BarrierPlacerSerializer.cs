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
        private readonly string newKey = "BarrierPlacerNew";

        public override void OnSaveData()
        {
            LoggerUtils.Log("Saving Barriers");

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream barrierMemoryStream = new MemoryStream();


            try
            {
                NewBarrierStrip[] barriers = BarrierManager.Instance().m_barrierList.ToArray();

                if (barriers != null)
                {
                    binaryFormatter.Serialize(barrierMemoryStream, barriers);
                    serializableDataManager.SaveData(newKey, barrierMemoryStream.ToArray());
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
            byte[] loadedNewBarrierData = serializableDataManager.LoadData(newKey);

            if (loadedBarrierData != null && loadedNewBarrierData == null)
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
                    LoggerUtils.LogWarning("Barriers Loaded");

                }
            }
            else
            {
                LoggerUtils.LogWarning("Found no data to load");
            }

            if (loadedNewBarrierData != null)
            {
                MemoryStream barrierMemoryStream = new MemoryStream();

                barrierMemoryStream.Write(loadedNewBarrierData, 0, loadedNewBarrierData.Length);
                barrierMemoryStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                try
                {
                    NewBarrierStrip[] barriers = binaryFormatter.Deserialize(barrierMemoryStream) as NewBarrierStrip[];

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
                    LoggerUtils.LogWarning("Barriers Loaded");

                }
            }
            else
            {
                LoggerUtils.LogWarning("Found no data to load");
            }

        }
    }
}
