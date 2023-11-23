from fastapi import FastAPI, UploadFile, File
from typing import List
import cv2
import numpy as np
import base64
from fastapi.responses import JSONResponse
from fastapi import UploadFile, File
from io import BytesIO

app = FastAPI()

@app.post("/image")
async def post(image: UploadFile = File(...)):
    """
    画像をグレースケールに変換する
    """
    contents = await image.read()
    nparr = np.frombuffer(contents, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    
    # 画像をグレースケールに変換
    gray_img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    # グレースケール画像をバイト列に変換
    _, img_encoded = cv2.imencode('.png', gray_img)
    img_bytes = img_encoded.tobytes()
    img_base64 = base64.b64encode(img_bytes).decode('utf-8')
    dataurl = 'data:image/png;base64,' + img_base64

    return JSONResponse(content={'img_data': dataurl})