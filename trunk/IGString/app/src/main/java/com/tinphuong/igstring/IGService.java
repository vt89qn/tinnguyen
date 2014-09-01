package com.tinphuong.igstring;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.widget.Toast;

import com.instagram.strings.StringBridge;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class IGService extends Service {
    private boolean bStopped =false;

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {
        Toast.makeText(this, "My Service Created", Toast.LENGTH_LONG).show();
    }

    @Override
    public void onDestroy() {
        Toast.makeText(this, "My Service Stopped", Toast.LENGTH_LONG).show();
        bStopped = true;
    }

    @Override
    public void onStart(Intent intent, int startid) {
        Toast.makeText(this, "My Service Started", Toast.LENGTH_LONG).show();
        bStopped = false;
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                signIGString();
            }
        });
        thread.start();
    }
    private void signIGString() {
        while (!bStopped) {
            try {
                URL url = new URL("http://api.tinphuong.com/?page=ig&stage=get");
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                conn.setReadTimeout(10000 /* milliseconds */);
                conn.setConnectTimeout(15000 /* milliseconds */);
                conn.setRequestMethod("GET");
                conn.setDoInput(true);
                // Starts the query
                conn.connect();
                InputStream stream = conn.getInputStream();

                String data = convertStreamToString(stream);
                stream.close();
                JSONArray listUnsignedString = new JSONArray(data);
                JSONArray listSignedString = new JSONArray();
                for (int i = 0; i < listUnsignedString.length(); i++) {
                    String strUnsignedString = listUnsignedString.getJSONObject(i).getString("unsignedstring");
                    JSONObject signed = new JSONObject();
                    signed.put("unsignedstring", strUnsignedString);
                    signed.put("signedstring", StringBridge.getSignatureString(strUnsignedString.getBytes()));
                    listSignedString.put(signed);
                }
                if (listSignedString.length() > 0) {
                    url = new URL("http://api.tinphuong.com/?page=ig&stage=set&signedstring=" + listSignedString.toString());
                    conn = (HttpURLConnection) url.openConnection();
                    conn.setReadTimeout(10000 /* milliseconds */);
                    conn.setConnectTimeout(15000 /* milliseconds */);
                    conn.setRequestMethod("GET");
                    conn.setDoInput(true);
                    // Starts the query
                    conn.connect();
                    stream = conn.getInputStream();
                    data = convertStreamToString(stream);
                    stream.close();
                } else {
                    Thread.sleep(5000);
                }
                Thread.sleep(1000);

            } catch (Exception e) {
                e.printStackTrace();
                bStopped = true;
            }
        }
    }

    private String convertStreamToString(java.io.InputStream is) {
        java.util.Scanner s = new java.util.Scanner(is).useDelimiter("\\A");
        return s.hasNext() ? s.next() : "";
    }
}
