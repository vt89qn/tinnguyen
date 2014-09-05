package com.tinphuong.igstring;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.TextView;
import android.widget.Toast;

import com.boyaa.common.BoyaaApp;
import com.instagram.strings.StringBridge;
import com.tinphuong.services.IGService;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.List;


public class IG extends Activity {
    private boolean bStopped =false;
    private int iCountGet =0;
    Handler handler = new Handler();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ig);
        getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.ig, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public void btnStart_Click(View v) {
        TextView lblStatus = (TextView) findViewById(R.id.lblStatus);
        lblStatus.setText("Started");
        //startService(new Intent(this, IGService.class));
        bStopped = false;
        iCountGet =0;
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                signIGString();
            }
        });
        thread.start();
    }

    public void btnStop_Click(View v) {
        TextView lblStatus = (TextView) findViewById(R.id.lblStatus);
        lblStatus.setText("Stopped");
        //stopService(new Intent(this, IGService.class));
        bStopped = true;
    }
    private void signIGString() {
        while (!bStopped) {
            try {
                URL url = new URL("http://api.tinphuong.com/?stage=get");
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
                    JSONObject jSon = listUnsignedString.getJSONObject(i);
                    String strUnsignedString = jSon.getString("unsignedstring");
                    String strPage = jSon.getString("page");
                    if (strPage.equalsIgnoreCase("ig")) {
                        JSONObject signed = new JSONObject();
                        signed.put("page","ig");
                        signed.put("unsignedstring", strUnsignedString);
                        signed.put("signedstring", StringBridge.getSignatureString(strUnsignedString.getBytes()));
                        listSignedString.put(signed);
                    } else if (strPage.equalsIgnoreCase("pk")) {
                        int iSeed = jSon.getInt("seed");
                        JSONObject signed = new JSONObject();
                        signed.put("page","pk");
                        signed.put("seed",iSeed);
                        signed.put("unsignedstring", strUnsignedString);
                        signed.put("signedstring1", new BoyaaApp().encode1(strUnsignedString.getBytes(),iSeed));
                        signed.put("signedstring2", new BoyaaApp().encode2(strUnsignedString.getBytes()));
                        listSignedString.put(signed);
                        Log.e("20", signed.toString());
                    }
                }
                if (listSignedString.length() > 0) {
                    HttpClient httpclient = new DefaultHttpClient();
                    HttpPost httppost = new HttpPost("http://api.tinphuong.com/Default.aspx");
                    try {
                        // Add your data
                        List<NameValuePair> params = new ArrayList<NameValuePair>();
                        params.add(new BasicNameValuePair("stage", "set"));
                        params.add(new BasicNameValuePair("signedstring", listSignedString.toString()));
                        httppost.setEntity(new UrlEncodedFormEntity(params));

                        // Execute HTTP Post Request
                        HttpResponse response = httpclient.execute(httppost);

                    } catch (ClientProtocolException e) {
                        // TODO Auto-generated catch block
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                    }
                }
                iCountGet++;
                handler.post(new Runnable(){
                    public void run() {
                        TextView lblStatus = (TextView) findViewById(R.id.lblStatus);
                        lblStatus.setText("Count :" + iCountGet);
                    }
                });
                Thread.sleep(3000);

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

    private String getQuery(List<NameValuePair> params) throws UnsupportedEncodingException
    {
        StringBuilder result = new StringBuilder();
        boolean first = true;

        for (NameValuePair pair : params)
        {
            if (first)
                first = false;
            else
                result.append("&");

            result.append(URLEncoder.encode(pair.getName(), "UTF-8"));
            result.append("=");
            result.append(URLEncoder.encode(pair.getValue(), "UTF-8"));
        }
        return result.toString();
    }
}
