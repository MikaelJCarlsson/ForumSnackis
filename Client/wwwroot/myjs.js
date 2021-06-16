        window.CreateCookie = (name, value)  => {

                document.cookie = name + "=" + value + "; path=/";
            }

            window.FindCookie = (name) => {

                var value = document.cookie.includes("ConsentCookie=true");
                return value;
            }
