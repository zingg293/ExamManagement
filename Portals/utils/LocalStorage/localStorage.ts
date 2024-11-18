import Cookies from 'js-cookie';

export const setLocalStorage = (key: string, value: string) => {
  Cookies.set(key, value);
};

export const getLocalStorage = (key: string) => {
  return Cookies.get(key);
};

export const removeLocalStorage = (key: string) => {
  Cookies.remove(key);
};
