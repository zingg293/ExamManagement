// useLocalStorage.js

import { useState, useEffect } from 'react';

export const useLocalStorage = (key: string, initialValue: string) => {
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    const storedValue = localStorage.getItem(key);
    if (storedValue) {
      setValue(JSON.parse(storedValue));
    }
  }, [key]);

  const updateValue = (newValue: string) => {
    setValue(newValue);
    localStorage.setItem(key, JSON.stringify(newValue));
  };

  const clearValue = () => {
    setValue('');
    localStorage.removeItem(key);
  };

  return [value, updateValue, clearValue];
};
