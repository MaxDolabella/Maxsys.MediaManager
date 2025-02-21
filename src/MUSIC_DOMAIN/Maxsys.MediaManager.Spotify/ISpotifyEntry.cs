namespace Maxsys.MediaManager.Spotify;

public interface ISpotifyEntry
{ }


/*

// Pre

// Base 64 encode client ID and client secret key to include in Auth Header
let client_id = pm.environment.get("spotify_client_id")
let client_secret = pm.environment.get("spotify_client_secret")
let stringToEncode = `${client_id}:${client_secret}`

let rawStr = CryptoJS.enc.Utf8.parse(stringToEncode)
let base64 = CryptoJS.enc.Base64.stringify(rawStr)
// console.log(`Encrypted value: ${base64}`)

// set local variable to be used in Auth Header
pm.environment.set("encodedIdAndKey", base64)


https://accounts.spotify.com/api/token
Form grant_type: client_credentials
Header: Auth: Basic {{encodedIdAndKey}}

// pos
const jsonData = pm.response.json();

if(jsonData.access_token){
    pm.environment.set("spotify_access_token", jsonData.access_token);
}


*/