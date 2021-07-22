export class ImageUploadAdapter {
  loader;
  reader;
  constructor(loader: any) {
    this.loader = loader;
  }

  public upload(): Promise<any> {
    return new Promise((resolve, reject) => {
      const reader = this.reader = new FileReader();

      reader.addEventListener('load', () => {
        resolve({ default: reader.result });
      });

      reader.addEventListener('error', err => {
        reject(err);
      });

      reader.addEventListener('abort', () => {
        reject();
      });

      this.loader.file.then(file => {
        reader.readAsDataURL(file);
      });
    });
  }
}
