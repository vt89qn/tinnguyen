// Decompiled by Jad v1.5.8e. Copyright 2001 Pavel Kouznetsov.
// Jad home page: http://www.geocities.com/kpdus/jad.html
// Decompiler options: braces fieldsfirst space lnc 

package com.boyaa.common;


import android.util.Log;

public class BoyaaApp
{
    public native String encode1(byte abyte0[], int i1);

    public native String encode2(byte abyte0[]);

    static {
        try {
            System.loadLibrary("poker_fb");

        } catch (UnsatisfiedLinkError ule) {
            Log.e("10", "******** Could not load native library nativeim ********");
            Log.e("10", "******** Could not load native library nativeim ********", ule);
            Log.e("10", "******** Could not load native library nativeim ********");
        } catch (Throwable t) {
            Log.e("10", "******** Failed to load native dictionary library ********");
            Log.e("10", "******** Failed to load native dictionary library *******", t);
            Log.e("10", "******** Failed to load native dictionary library ********");
        }
    }
}
