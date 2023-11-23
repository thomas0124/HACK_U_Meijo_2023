from fastapi import FastAPI, UploadFile, File
from typing import List
import json

app = FastAPI()

@app.post("/uploadfiles/")
async def create_upload_files(files: List[UploadFile] = File(...)):
    # 画像処理のコードをここに書く
    # 得られた結果に応じて必殺技を割り振るコードをここに書く
    name = "test"  # これは例です。実際のコードでは、画像処理の結果に基づいて必殺技を割り振る必要があります。
    return {"name": name}
