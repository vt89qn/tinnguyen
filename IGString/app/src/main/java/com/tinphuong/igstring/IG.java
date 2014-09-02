package com.tinphuong.igstring;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

import com.tinphuong.services.IGService;


public class IG extends Activity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ig);
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
        startService(new Intent(this, IGService.class));
    }

    public void btnStop_Click(View v) {
        TextView lblStatus = (TextView) findViewById(R.id.lblStatus);
        lblStatus.setText("Stopped");
        stopService(new Intent(this, IGService.class));
    }
}
