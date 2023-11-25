from fastapi import FastAPI, File, UploadFile

app = FastAPI()

@app.get("/")
def read_root():
    return {"message": "Hello World!"}

@app.post("/uploadfile/")
async def create_upload_file(file: UploadFile = File(...)):
    return {"filename": file.filename}

@app.post("/uploadfile/{id}")
async def create_upload_file(id: int, file: UploadFile = File(...)):
    return {"filename": file.filename, "id": id}