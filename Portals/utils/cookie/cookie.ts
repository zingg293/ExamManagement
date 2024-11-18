// get cookie name
export const getCookie = (name: string | any[]) => {
  let cookieValue = null;
  if (
    typeof window !== 'undefined' &&
    document.cookie &&
    document.cookie !== ''
  ) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i].trim();
      // Does this cookie string begin with the name we want?
      if (cookie.substring(0, name.length + 1) === name + '=') {
        cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
        break;
      }
    }
  }
  return cookieValue;
};

// add a cookie
export const setCookie = (name: string, value: string, days = Date.now()) => {
  let expires = '';
  if (days) {
    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    expires = '; expires=' + date.toUTCString();
  }
  if (typeof window !== 'undefined') {
    document.cookie = name + '=' + (value || '') + expires + '; path=/';
  }
};

// delete a cookie
export const deleteCookie = (name: string) => {
  setCookie(name, '', -1);
};
