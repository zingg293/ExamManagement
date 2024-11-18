import { all } from "redux-saga/effects";

export default function* rootSaga() {
  console.log("rootSaga", "Hello");
  yield all([]);
}
