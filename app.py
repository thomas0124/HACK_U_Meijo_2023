from flask import Flask, jsonify, request
from flask_cors import CORS
import cv2
import numpy as np
import base64
import json

app = Flask('flask-tesseract-api')
CORS(app)

@app.route("/image", methods=["POST"])
def post():
    """
    画像をグレースケールに変換する
    """
    # Imageをデコード
    img_stream = request.files.get("image").stream
    # 配列に変換
    img_array = np.asarray(bytearray(img_stream.read()), dtype=np.uint8)
    img = cv2.imdecode(img_array, 1)
    
    # 画像をグレースケールに変換
    gray_img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    # 変換結果を保存
    cv2.imwrite('result_gray.png', gray_img)
    
    # 保存したファイルに対してエンコード
    with open('result_gray.png', "rb") as f:
        img_base64 = base64.b64encode(f.read()).decode('utf-8')
        dataurl = 'data:image/png;base64,' + img_base64

    return jsonify({'img_data': dataurl})


if __name__ == '__main__':
    app.run(debug=True)
