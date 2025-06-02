import requests
import os
import pandas as pd

API_KEY = 'dba6f327-ab36-11ef-9d14-fe308d538ab7'  
MODEL_ID = '33361d67-7adf-4b64-bbf9-ac0fe0755c6e' 
URL = f'https://app.nanonets.com/api/v2/OCR/Model/{MODEL_ID}/LabelFile/'

# OCR yapılacak görselin yolu
image_path = r'C:\Users\oztur\Desktop\ders programı.png'

# Kaydedilecek klasör
save_folder = r"C:\Users\oztur\Desktop"
save_folder_csv = r"Assets/StreamingAssets"

# Görsel var mı kontrolü
if not os.path.exists(image_path):
    raise FileNotFoundError(f"Dosya bulunamadı: {image_path}")

# Kayıt klasörü yoksa oluştur
os.makedirs(save_folder, exist_ok=True)

# API çağrısı
def upload_image(image_path):
    print(f"API'ye gönderilen dosya yolu: {image_path}")
    files = {'file': open(image_path, 'rb')}
    response = requests.post(URL, auth=(API_KEY, ''), files=files)
    
    if response.status_code == 200:
        print("API çağrısı başarılı!")
        return response.json()
    else:
        print(f"Hata oluştu: {response.status_code}")
        print(f"Detaylı hata mesajı: {response.text}")
        return None

# JSON sonucu
response_data = upload_image(image_path)

if response_data:
    print("OCR İşlemi Sonucu:")
    print(response_data)
else:
    print("OCR işlemi başarısız oldu.")

# JSON'u düzenleme ve Excel/CSV olarak kaydetme
try:
    cells = response_data["result"][0]["prediction"][0]["cells"]

    # Tablo verilerini işleme
    table_data = {}

    for cell in cells:
        row = cell["row"]
        col = cell["col"]
        text = cell["text"]

        if row not in table_data:
            table_data[row] = {}

        table_data[row][col] = text

    # DataFrame'e dönüştürme
    df = pd.DataFrame.from_dict(table_data, orient="index")
    df.fillna("", inplace=True)

    # 📋 Terminale tabloyu yazdır
    print("\nOCR'den elde edilen tablo verisi:")
    print(df)

    # 📁 Dosya yolları
    csv_path = os.path.join(save_folder_csv, "ders_program_cıktı.csv")
    excel_path = os.path.join(save_folder, "ders_p_cikti.xlsx")

    # 💾 Kaydetme
    df.to_csv(csv_path, index=False)
    print(f"CSV dosyası başarıyla kaydedildi: {csv_path}")

    df.to_excel(excel_path, index=False)
    print(f"Excel dosyası başarıyla kaydedildi: {excel_path}")

except KeyError as e:
    print(f"JSON yapısında beklenmeyen bir anahtar: {e}")
except Exception as e:
    print(f"Beklenmeyen bir hata oluştu: {e}")
