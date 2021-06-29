export interface IModelAdapter<T> {
  Adapt(data: any): T;
}
