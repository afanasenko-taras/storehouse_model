import pandas as pd
import os
import seaborn as sns
import matplotlib.pyplot as plt
sns.set()

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    df = pd.read_csv('result.csv')
    test = df.where(df.charge==10).where(df.rotate_time==4)
    plt.plot(test.robot, test.time/3600)
    plt.title("Time to deliver 5000 parcels ")
    plt.xlabel("number of robots")
    plt.ylabel("hours")
    plt.show()